import { Component, Input, Output, EventEmitter, ViewChild, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AgGridModule, AgGridAngular } from 'ag-grid-angular';
import { ColDef, GridApi, GridReadyEvent } from 'ag-grid-community';
import { NbCardModule, NbIconModule, NbButtonModule, NbDialogService } from '@nebular/theme';
import { FormFieldConfig } from '../../../@core/models/form-field.models';
import { SharedDialogComponent } from '../shared-dialogs/shared-dialog.component';
import { TranslatePipe } from '../../../@core/pipes/translate.pipe';

@Component({
    selector: 'app-ag-grid-table',
    templateUrl: './ag-grid-table.component.html',
    styleUrls: ['./ag-grid-table.component.scss'],
    standalone: true,
    imports: [CommonModule, AgGridModule, NbCardModule, NbIconModule, NbButtonModule, TranslatePipe]
})
export class AgGridTableComponent implements OnInit, OnChanges {
    @ViewChild(AgGridAngular) agGrid!: AgGridAngular;

    @Input() columnDefs: ColDef[] = [];
    @Input() rowData: any[] = [];
    @Input() title: string = 'Tabela';
    @Input() pagination: boolean = true;
    @Input() paginationPageSize: number = 10;
    @Input() localeText: any = {};
    @Input() formFields: FormFieldConfig[] = [];
    @Input() showTitle: boolean = true;
    @Input() loading: boolean = false;

    @Output() onAdd = new EventEmitter<any>();
    @Output() onEdit = new EventEmitter<{ original: any; updated: any }>();
    @Output() onDeleteSelected = new EventEmitter<any[]>();

    private gridApi!: GridApi;
    selectedRows: any[] = [];

    defaultColDef: ColDef = {
        sortable: true,
        filter: true,
        resizable: true,
    };

    overlayLoadingTemplate = '<div class="ag-overlay-loading-center"><div class="loading-spinner"></div><span>Carregando...</span></div>';
    overlayNoRowsTemplate = '<span>Nenhum registro encontrado.</span>';

    getRowStyle = (params: any) => {
        if (params.node.rowIndex % 2 === 0) {
            return { 'background-color': '#f0f0f0' };
        }
        return { 'background-color': '#ffffff' };
    };

    constructor(private dialogService: NbDialogService) {}

    ngOnChanges(changes: SimpleChanges): void {
        if (changes['loading'] && this.gridApi) {
            if (this.loading) {
                this.gridApi.showLoadingOverlay();
            } else {
                this.gridApi.hideOverlay();
            }
        }
    }

    ngOnInit(): void {
        if (this.columnDefs.length > 0) {
            const lastCol = this.columnDefs[this.columnDefs.length - 1];
            lastCol.suppressMovable = true;
            lastCol.resizable = false;
        }
    }

    onGridReady(params: GridReadyEvent): void {
        this.gridApi = params.api;
    }

    onSelectionChanged(): void {
        this.selectedRows = this.gridApi ? this.gridApi.getSelectedRows() : [];
    }

    refreshData(newData: any[]): void {
        if (this.gridApi) {
            this.rowData = newData;
            this.gridApi.setGridOption('rowData', newData);
        }
    }

    getSelectedRows(): any[] {
        return this.gridApi ? this.gridApi.getSelectedRows() : [];
    }

    exportCSV(): void {
        if (this.gridApi) {
            this.gridApi.exportDataAsCsv({
                fileName: `${this.title.toLowerCase()}_export.csv`,
                columnSeparator: ';',
                onlySelected: this.selectedRows.length > 0,
            });
        }
    }

    // ---- TOOLBAR ACTIONS ----

    onToolbarAdd(): void {
        this.dialogService.open(SharedDialogComponent, {
            context: {
                mode: 'form',
                title: this.title,
                fields: this.formFields,
                data: null,
                buttonStatus: 'info',
            },
        }).onClose.subscribe((result: any) => {
            if (result) {
                this.onAdd.emit(result);
            }
        });
    }

    onToolbarEdit(): void {
        if (this.selectedRows.length === 1) {
            const original = this.selectedRows[0];
            this.dialogService.open(SharedDialogComponent, {
                context: {
                    mode: 'form',
                    title: this.title,
                    fields: this.formFields,
                    data: original,
                    buttonStatus: 'warning',
                },
            }).onClose.subscribe((result: any) => {
                if (result) {
                    this.onEdit.emit({ original, updated: result });
                }
            });
        }
    }

    onToolbarDelete(): void {
        if (this.selectedRows.length > 0) {
            this.dialogService.open(SharedDialogComponent, {
                context: {
                    mode: 'delete',
                    title: this.title,
                    count: this.selectedRows.length,
                    buttonStatus: 'danger',
                },
            }).onClose.subscribe((confirmed: boolean) => {
                if (confirmed) {
                    this.onDeleteSelected.emit(this.selectedRows);
                }
            });
        }
    }

    onToolbarExport(): void {
        this.dialogService.open(SharedDialogComponent, {
            context: {
                mode: 'export',
                title: this.title,
                count: this.selectedRows.length,
                total: this.rowData.length,
                buttonStatus: 'success',
            },
        }).onClose.subscribe((confirmed: boolean) => {
            if (confirmed) {
                this.exportCSV();
            }
        });
    }
}
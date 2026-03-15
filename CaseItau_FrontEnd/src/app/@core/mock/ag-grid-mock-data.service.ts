import { Injectable } from '@angular/core';
import { Canal, Cliente, Integracao, Usuario, Produto, Pedido } from '../models/ag-grid.models';

@Injectable({ providedIn: 'root' })
export class AgGridMockDataService {
    private canaisStorageKey = 'sabr_canais_data';
    private clientesStorageKey = 'sabr_clientes_data';
    private integracoesStorageKey = 'sabr_integracoes_data';
    private usuariosStorageKey = 'sabr_usuarios_data';
    private produtosStorageKey = 'sabr_produtos_data';
    private pedidosStorageKey = 'sabr_pedidos_data';
    private canais: Canal[] = [];
    private clientes: Cliente[] = [];
    private integracoes: Integracao[] = [];
    private usuarios: Usuario[] = [];
    private produtos: Produto[] = [];
    private pedidos: Pedido[] = [];


    constructor() {
        this.loadCanaisData();
        this.loadClientesData();
        this.loadIntegracoesData();
        this.loadUsuariosData();
        this.loadProdutosData();
        this.loadPedidosData();
    }

    // ========== CANAIS ==========
    getCanaisData(): Canal[] {
        return [...this.canais];
    }

    addCanal(record: Omit<Canal, 'id' | 'created_at' | 'created_by'>): void {
        const newId = this.canais.length > 0 ? Math.max(...this.canais.map(c => c.id)) + 1 : 1;
        const newCanal: Canal = {
            ...record,
            id: newId,
            created_at: new Date().toISOString(),
            created_by: 'admin'
        };
        this.canais.push(newCanal);
        this.saveCanaisData();
    }

    updateCanal(id: number, record: Partial<Canal>): void {
        const index = this.canais.findIndex(c => c.id === id);
        if (index !== -1) {
            this.canais[index] = { ...this.canais[index], ...record, id };
            this.saveCanaisData();
        }
    }

    deleteCanal(id: number): void {
        this.canais = this.canais.filter(c => c.id !== id);
        this.saveCanaisData();
    }

    private loadCanaisData(): void {
        const stored = localStorage.getItem(this.canaisStorageKey);
        if (stored) {
            this.canais = JSON.parse(stored);
        } else {
            this.canais = [
                {
                    id: 1,
                    type: 'marketplace',
                    name: 'Mercado Livre',
                    shortened_name: 'MELI',
                    image_link: 'assets/images/canais/mercadolivre.png',
                    created_at: '2024-07-30 07:02:15',
                    created_by: 'admin',
                },
                {
                    id: 2,
                    type: 'marketplace',
                    name: 'Shopee',
                    shortened_name: 'SHPE',
                    image_link: 'assets/images/canais/shopee.png',
                    created_at: '2024-07-30 07:02:25',
                    created_by: 'admin',
                },
                {
                    id: 3,
                    type: 'marketplace',
                    name: 'Shopify',
                    shortened_name: 'SHFY',
                    image_link: 'assets/images/canais/shopify.png',
                    created_at: '2024-07-30 07:02:33',
                    created_by: 'admin',
                },
                {
                    id: 4,
                    type: 'marketplace',
                    name: 'Loja Integrada',
                    shortened_name: 'LOIT',
                    image_link: 'assets/images/canais/lojaintegrada.png',
                    created_at: '2024-07-30 07:04:23',
                    created_by: 'admin',
                },
                {
                    id: 5,
                    type: 'erp',
                    name: 'Pedido Manual',
                    shortened_name: 'MANU',
                    image_link: 'assets/images/canais/manual.png',
                    created_at: '2024-08-13 06:58:51',
                    created_by: 'admin',
                },
                {
                    id: 6,
                    type: 'marketplace',
                    name: 'Bling V3',
                    shortened_name: 'BLIN',
                    image_link: 'assets/images/canais/bling.png',
                    created_at: '2024-08-19 15:14:10',
                    created_by: 'admin',
                },
                {
                    id: 7,
                    type: 'marketplace',
                    name: 'Nuvemshop',
                    shortened_name: 'NUSH',
                    image_link: 'assets/images/canais/nuvemshop.png',
                    created_at: '2024-09-23 08:40:37',
                    created_by: 'admin',
                },
                {
                    id: 8,
                    type: 'marketplace',
                    name: 'Yampi',
                    shortened_name: 'YAMP',
                    image_link: 'assets/images/canais/yampi.png',
                    created_at: '2024-09-30 10:08:39',
                    created_by: 'admin',
                },

            ];
            this.saveCanaisData();
        }
    }

    private saveCanaisData(): void {
        localStorage.setItem(this.canaisStorageKey, JSON.stringify(this.canais));
    }

    // ========== CLIENTES (exemplo para futuro) ==========
    getClientesData(): Cliente[] {
        return [...this.clientes];
    }

    addCliente(record: Omit<Cliente, 'id'>): void {
        const newId = this.clientes.length > 0 ? Math.max(...this.clientes.map(c => c.id)) + 1 : 1;
        const newCliente: Cliente = { ...record, id: newId };
        this.clientes.push(newCliente);
        this.saveClientesData();
    }

    updateCliente(id: number, record: Partial<Cliente>): void {
        const index = this.clientes.findIndex(c => c.id === id);
        if (index !== -1) {
            this.clientes[index] = { ...this.clientes[index], ...record, id };
            this.saveClientesData();
        }
    }

    deleteCliente(id: number): void {
        this.clientes = this.clientes.filter(c => c.id !== id);
        this.saveClientesData();
    }

    private loadClientesData(): void {
        const stored = localStorage.getItem(this.clientesStorageKey);
        if (stored) {
            this.clientes = JSON.parse(stored);
        } else {
            this.clientes = [
                { id: 1, name: 'Mark Otto', storeName: 'Loja do Mark', whatsapp: '11987654321', email: 'mark@gmail.com', currentBalance: '28.40' },
                { id: 2, name: 'Jacob Thornton', storeName: 'JT Store', whatsapp: '11983451234', email: 'jacob@gmail.com', currentBalance: '45.12' },
                { id: 3, name: 'Larry Bird', storeName: 'Bird Shop', whatsapp: '11992345678', email: 'larry@outlook.com', currentBalance: '18.90' },
                { id: 4, name: 'John Snow', storeName: 'Winter Store', whatsapp: '11994561234', email: 'john@gmail.com', currentBalance: '20.33' },
                { id: 5, name: 'Jack Sparrow', storeName: 'Black Pearl', whatsapp: '11991239876', email: 'jack@gmail.com', currentBalance: '30.05' },
                { id: 6, name: 'Ana Silva', storeName: 'Silva Modas', whatsapp: '11998877665', email: 'ana@gmail.com', currentBalance: '21.77' },
                { id: 7, name: 'Barbara Santos', storeName: 'BS Acessórios', whatsapp: '11990011223', email: 'barbara@gmail.com', currentBalance: '43.61' },
                { id: 8, name: 'Carlos Ferreira', storeName: 'CF Eletrônicos', whatsapp: '11995556677', email: 'carlos@outlook.com', currentBalance: '13.19' },
                { id: 9, name: 'Daniela Costa', storeName: 'DC Cosméticos', whatsapp: '11996667788', email: 'daniela@gmail.com', currentBalance: '22.84' },
                { id: 10, name: 'Eduardo Lima', storeName: 'Lima Tech', whatsapp: '11997778899', email: 'eduardo@gmail.com', currentBalance: '33.28' },
                { id: 11, name: 'Fernanda Oliveira', storeName: 'FO Bijoux', whatsapp: '11981112233', email: 'fernanda@gmail.com', currentBalance: '38.52' },
                { id: 12, name: 'Gabriel Souza', storeName: 'Souza Games', whatsapp: '11982223344', email: 'gabriel@outlook.com', currentBalance: '48.07' },
                { id: 13, name: 'Helena Martins', storeName: 'HM Fashion', whatsapp: '11983334455', email: 'helena@gmail.com', currentBalance: '48.48' },
                { id: 14, name: 'Igor Pereira', storeName: 'IP Sports', whatsapp: '11984445566', email: 'igor@gmail.com', currentBalance: '40.11' },
                { id: 15, name: 'Julia Almeida', storeName: 'JA Decorações', whatsapp: '11985556677', email: 'julia@outlook.com', currentBalance: '32.95' },
                { id: 16, name: 'Lucas Ribeiro', storeName: 'LR Calçados', whatsapp: '11986667788', email: 'lucas@gmail.com', currentBalance: '11.30' },
                { id: 17, name: 'Mariana Gomes', storeName: 'MG Papelaria', whatsapp: '11987778899', email: 'mariana@gmail.com', currentBalance: '34.64' },
                { id: 18, name: 'Nicolas Barbosa', storeName: 'NB Imports', whatsapp: '11988889900', email: 'nicolas@outlook.com', currentBalance: '45.18' },
                { id: 19, name: 'Olivia Rocha', storeName: 'OR Natural', whatsapp: '11989990011', email: 'olivia@gmail.com', currentBalance: '32.72' },
                { id: 20, name: 'Pedro Carvalho', storeName: 'PC Informática', whatsapp: '11980001122', email: 'pedro@gmail.com', currentBalance: '19.09' },
            ];
            this.saveClientesData();
        }
    }

    private saveClientesData(): void {
        localStorage.setItem(this.clientesStorageKey, JSON.stringify(this.clientes));
    }


    // ========== INTEGRAÇÕES ==========
    getIntegracoesData(): Integracao[] {
        return [...this.integracoes];
    }

    addIntegracao(record: Omit<Integracao, 'id' | 'created_at' | 'created_by'>): void {
        const newId = this.integracoes.length > 0 ? Math.max(...this.integracoes.map(i => i.id)) + 1 : 1;
        const newIntegracao: Integracao = {
            ...record,
            id: newId,
            created_at: new Date().toISOString(),
            created_by: 'admin'
        };
        this.integracoes.push(newIntegracao);
        this.saveIntegracoesData();
    }

    updateIntegracao(id: number, record: Partial<Integracao>): void {
        const index = this.integracoes.findIndex(i => i.id === id);
        if (index !== -1) {
            this.integracoes[index] = { ...this.integracoes[index], ...record, id };
            this.saveIntegracoesData();
        }
    }

    deleteIntegracao(id: number): void {
        this.integracoes = this.integracoes.filter(i => i.id !== id);
        this.saveIntegracoesData();
    }

    private loadIntegracoesData(): void {
        const stored = localStorage.getItem(this.integracoesStorageKey);
        if (stored) {
            this.integracoes = JSON.parse(stored);
        } else {
            this.integracoes = [
                {
                    id: 1,
                    channel_id: 1,
                    client_id: 1,
                    store_name: 'Loja Mercado Livre',
                    store_description: 'Integração ML principal',
                    api_key: 'APP-1234567890',
                    client_key: 'TXZ-987654',
                    client_secret: '********',
                    status: true,
                    created_at: '2024-07-30 10:00:00',
                    created_by: 'admin',
                },
                {
                    id: 2,
                    channel_id: 2,
                    client_id: 1,
                    store_name: 'Loja Shopee',
                    store_description: 'Integração Shopee BR',
                    api_key: 'SHP-0987654321',
                    client_key: 'SPE-123456',
                    client_secret: '********',
                    status: true,
                    created_at: '2024-08-15 14:30:00',
                    created_by: 'admin',
                },
                {
                    id: 3,
                    channel_id: 3,
                    client_id: 2,
                    store_name: 'Shopify Store',
                    store_description: 'Loja internacional',
                    api_key: 'SHFY-1122334455',
                    client_key: 'SFY-654321',
                    client_secret: '********',
                    status: false,
                    created_at: '2024-09-01 09:15:00',
                    created_by: 'admin',
                },
            ];
            this.saveIntegracoesData();
        }
    }

    private saveIntegracoesData(): void {
        localStorage.setItem(this.integracoesStorageKey, JSON.stringify(this.integracoes));
    }

    // ========== USUÁRIOS ==========
    getUsuariosData(): Usuario[] {
        return [...this.usuarios];
    }

    findUserByEmail(email: string): Usuario | undefined {
        return this.usuarios.find(u => u.email.toLowerCase() === email.toLowerCase());
    }

    addUsuario(record: Omit<Usuario, 'id' | 'created_at' | 'created_by'>): void {
        const newId = this.usuarios.length > 0 ? Math.max(...this.usuarios.map(u => u.id)) + 1 : 1;
        const newUsuario: Usuario = {
            ...record,
            id: newId,
            created_at: new Date().toISOString(),
            created_by: 'admin'
        };
        this.usuarios.push(newUsuario);
        this.saveUsuariosData();
    }

    updateUsuario(id: number, record: Partial<Usuario>): void {
        const index = this.usuarios.findIndex(u => u.id === id);
        if (index !== -1) {
            this.usuarios[index] = { ...this.usuarios[index], ...record, id };
            this.saveUsuariosData();
        }
    }

    deleteUsuario(id: number): void {
        this.usuarios = this.usuarios.filter(u => u.id !== id);
        this.saveUsuariosData();
    }

    private loadUsuariosData(): void {
        const stored = localStorage.getItem(this.usuariosStorageKey);
        if (stored) {
            this.usuarios = JSON.parse(stored);

            // Migrar senhas de '********' para '123456' (migração única)
            const needsMigration = this.usuarios.some(u => u.password === '********');
            if (needsMigration) {
                this.usuarios.forEach(u => {
                    if (u.password === '********') {
                        u.password = '123456';
                    }
                });
                this.saveUsuariosData();
            }
        } else {
            this.usuarios = [
                { id: 1, name: 'Admin Sistema', store_name: '', person_type: 'PF', document: '123.456.789-00', ie_exempt: true, ie: '', email: 'admin@sabr.com', whatsapp: '11999990001', address_zip: '01001-000', address_street: 'Rua da Empresa', address_number: 100, address_complement: 'Sala 1', address_neighborhood: 'Centro', address_city: 'São Paulo', address_state: 'SP', password: '123456', profile: 'interno', status: true, current_balance: 0, is_blocked: false, created_at: '2024-01-01 08:00:00', created_by: 'sistema' },
                { id: 2, name: 'Carlos Operador', store_name: '', person_type: 'PF', document: '234.567.890-11', ie_exempt: true, ie: '', email: 'carlos@sabr.com', whatsapp: '11999990002', address_zip: '01002-000', address_street: 'Rua da Empresa', address_number: 100, address_complement: 'Sala 2', address_neighborhood: 'Centro', address_city: 'São Paulo', address_state: 'SP', password: '123456', profile: 'interno', status: true, current_balance: 0, is_blocked: false, created_at: '2024-02-15 09:00:00', created_by: 'admin' },
                { id: 3, name: 'Mark Otto', store_name: 'Loja do Mark', person_type: 'PF', document: '345.678.901-22', ie_exempt: false, ie: '123456789', email: 'mark@gmail.com', whatsapp: '11987654321', address_zip: '04001-000', address_street: 'Av. Paulista', address_number: 1500, address_complement: 'Apto 42', address_neighborhood: 'Bela Vista', address_city: 'São Paulo', address_state: 'SP', password: '123456', profile: 'cliente', status: true, current_balance: 28.40, is_blocked: false, created_at: '2024-03-01 10:00:00', created_by: 'admin' },
                { id: 4, name: 'Ana Silva', store_name: 'Silva Modas', person_type: 'PJ', document: '12.345.678/0001-90', ie_exempt: false, ie: '987654321', email: 'ana@gmail.com', whatsapp: '11998877665', address_zip: '20040-020', address_street: 'Rua do Comércio', address_number: 250, address_complement: '', address_neighborhood: 'Centro', address_city: 'Rio de Janeiro', address_state: 'RJ', password: '123456', profile: 'cliente', status: true, current_balance: 21.77, is_blocked: false, created_at: '2024-04-10 11:00:00', created_by: 'admin' },
                { id: 5, name: 'Pedro Carvalho', store_name: 'PC Informática', person_type: 'PJ', document: '98.765.432/0001-10', ie_exempt: true, ie: '', email: 'pedro@gmail.com', whatsapp: '11980001122', address_zip: '30130-000', address_street: 'Av. Afonso Pena', address_number: 800, address_complement: 'Loja 3', address_neighborhood: 'Centro', address_city: 'Belo Horizonte', address_state: 'MG', password: '123456', profile: 'cliente', status: false, current_balance: 19.09, is_blocked: false, created_at: '2024-05-20 14:00:00', created_by: 'admin' },
                { id: 6, name: 'Gestor Financeiro', store_name: '', person_type: 'PF', document: '456.789.012-33', ie_exempt: true, ie: '', email: 'financeiro@sabr.com', whatsapp: '11999990003', address_zip: '01001-000', address_street: 'Rua da Empresa', address_number: 100, address_complement: 'Sala 3', address_neighborhood: 'Centro', address_city: 'São Paulo', address_state: 'SP', password: '123456', profile: 'interno', status: true, current_balance: 0, is_blocked: false, created_at: '2024-01-15 08:30:00', created_by: 'admin' },
                { id: 7, name: 'Gabriel Souza', store_name: 'Souza Games', person_type: 'PF', document: '567.890.123-44', ie_exempt: true, ie: '', email: 'gabriel@outlook.com', whatsapp: '11982223344', address_zip: '80010-000', address_street: 'Rua XV de Novembro', address_number: 300, address_complement: '', address_neighborhood: 'Centro', address_city: 'Curitiba', address_state: 'PR', password: '123456', profile: 'cliente', status: true, current_balance: 48.07, is_blocked: false, created_at: '2024-06-01 10:30:00', created_by: 'admin' },
                { id: 8, name: 'Helena Martins', store_name: 'HM Fashion', person_type: 'PJ', document: '11.222.333/0001-44', ie_exempt: false, ie: '112233445', email: 'helena@gmail.com', whatsapp: '11983334455', address_zip: '90010-000', address_street: 'Rua dos Andradas', address_number: 450, address_complement: 'Sala 10', address_neighborhood: 'Centro', address_city: 'Porto Alegre', address_state: 'RS', password: '123456', profile: 'cliente', status: true, current_balance: 48.48, is_blocked: false, created_at: '2024-07-15 09:00:00', created_by: 'admin' },
                { id: 9, name: 'Igor Pereira', store_name: 'IP Sports', person_type: 'PF', document: '678.901.234-55', ie_exempt: true, ie: '', email: 'igor@gmail.com', whatsapp: '11984445566', address_zip: '40020-000', address_street: 'Av. Sete de Setembro', address_number: 600, address_complement: '', address_neighborhood: 'Centro', address_city: 'Salvador', address_state: 'BA', password: '123456', profile: 'cliente', status: true, current_balance: 40.11, is_blocked: true, created_at: '2024-08-01 13:00:00', created_by:'admin' },
                { id : 10, name : "Suporte Técnico", store_name : "", person_type : 'PF', document : "789.012.345-66", ie_exempt : true, ie : "", email : "suporte@sabr.com", whatsapp : "11999990004", address_zip : "01001-000", address_street : "Rua da Empresa", address_number : 100, address_complement : "Sala 4", address_neighborhood : "Centro", address_city : "São Paulo", address_state : "SP", password : "123456", profile : "interno", status : true, current_balance : 0, is_blocked : false, created_at :"2024-02-01 08:00:00", created_by :"admin" },
            ];
            this.saveUsuariosData();
        }
    }

    private saveUsuariosData(): void {
        localStorage.setItem(this.usuariosStorageKey, JSON.stringify(this.usuarios));
    }

    // ========== PRODUTOS ==========
    getProdutosData(): Produto[] {
        return [...this.produtos];
    }

    addProduto(record: Omit<Produto, 'id'>): void {
        const newId = this.produtos.length > 0 ? Math.max(...this.produtos.map(p => p.id)) + 1 : 1;
        const newProduto: Produto = { ...record, id: newId };
        this.produtos.push(newProduto);
        this.saveProdutosData();
    }

    updateProduto(id: number, record: Partial<Produto>): void {
        const index = this.produtos.findIndex(p => p.id === id);
        if (index !== -1) {
            this.produtos[index] = { ...this.produtos[index], ...record, id };
            this.saveProdutosData();
        }
    }

    deleteProduto(id: number): void {
        this.produtos = this.produtos.filter(p => p.id !== id);
        this.saveProdutosData();
    }

    private loadProdutosData(): void {
        const stored = localStorage.getItem(this.produtosStorageKey);
        if (stored) {
            this.produtos = JSON.parse(stored);
        } else {
            this.produtos = [
                { id: 1, name: 'Camiseta Básica', price: 49.90, quantity: 150 },
                { id: 2, name: 'Calça Jeans Slim', price: 129.90, quantity: 80 },
                { id: 3, name: 'Tênis Casual', price: 199.90, quantity: 45 },
                { id: 4, name: 'Mochila Escolar', price: 89.90, quantity: 200 },
                { id: 5, name: 'Relógio Digital', price: 159.90, quantity: 60 },
            ];
            this.saveProdutosData();
        }
    }

    private saveProdutosData(): void {
        localStorage.setItem(this.produtosStorageKey, JSON.stringify(this.produtos));
    }

    // ========== PEDIDOS ==========
    getPedidosData(): Pedido[] {
        return [...this.pedidos];
    }

    addPedido(record: Omit<Pedido, 'id' | 'created_at'>): void {
        const newId = this.pedidos.length > 0 ? Math.max(...this.pedidos.map(p => p.id)) + 1 : 1;
        const newPedido: Pedido = { ...record, id: newId, created_at: new Date().toISOString() };
        this.pedidos.push(newPedido);
        this.savePedidosData();
    }

    updatePedido(id: number, record: Partial<Pedido>): void {
        const index = this.pedidos.findIndex(p => p.id === id);
        if (index !== -1) {
            this.pedidos[index] = { ...this.pedidos[index], ...record, id };
            this.savePedidosData();
        }
    }

    deletePedido(id: number): void {
        this.pedidos = this.pedidos.filter(p => p.id !== id);
        this.savePedidosData();
    }

    private loadPedidosData(): void {
        const stored = localStorage.getItem(this.pedidosStorageKey);
        if (stored) {
            this.pedidos = JSON.parse(stored);
        } else {
            this.pedidos = [
                { id: 1, order_number: 'PED-001', client_name: 'Mark Otto', channel: 'Shopee', status: 'pago', total: 149.90, created_at: '2025-03-01 10:15:00' },
                { id: 2, order_number: 'PED-002', client_name: 'Ana Silva', channel: 'Mercado Livre', status: 'pago', total: 259.80, created_at: '2025-03-01 14:30:00' },
                { id: 3, order_number: 'PED-003', client_name: 'Gabriel Souza', channel: 'Shopify', status: 'cancelado', total: 89.90, created_at: '2025-03-02 08:45:00' },
                { id: 4, order_number: 'PED-004', client_name: 'Helena Martins', channel: 'Shopee', status: 'pendente', total: 499.70, created_at: '2025-03-02 11:00:00' },
                { id: 5, order_number: 'PED-005', client_name: 'Pedro Carvalho', channel: 'Nuvemshop', status: 'pago', total: 199.90, created_at: '2025-03-03 09:20:00' },
                { id: 6, order_number: 'PED-006', client_name: 'Igor Pereira', channel: 'Mercado Livre', status: 'cancelado', total: 329.70, created_at: '2025-03-03 16:10:00' },
                { id: 7, order_number: 'PED-007', client_name: 'Mark Otto', channel: 'Shopee', status: 'pago', total: 79.90, created_at: '2025-03-04 10:00:00' },
                { id: 8, order_number: 'PED-008', client_name: 'Ana Silva', channel: 'Bling V3', status: 'pendente', total: 649.50, created_at: '2025-03-04 13:45:00' },
                { id: 9, order_number: 'PED-009', client_name: 'Gabriel Souza', channel: 'Yampi', status: 'pago', total: 159.90, created_at: '2025-03-05 08:30:00' },
                { id: 10, order_number: 'PED-010', client_name: 'Helena Martins', channel: 'Shopee', status: 'pago', total: 289.80, created_at: '2025-03-05 15:20:00' },
                { id: 11, order_number: 'PED-011', client_name: 'Pedro Carvalho', channel: 'Mercado Livre', status: 'cancelado', total: 119.90, created_at: '2025-03-06 09:00:00' },
                { id: 12, order_number: 'PED-012', client_name: 'Igor Pereira', channel: 'Shopify', status: 'pendente', total: 399.90, created_at: '2025-03-06 14:15:00' },
                { id: 13, order_number: 'PED-013', client_name: 'Mark Otto', channel: 'Loja Integrada', status: 'pago', total: 549.70, created_at: '2025-03-07 10:30:00' },
                { id: 14, order_number: 'PED-014', client_name: 'Ana Silva', channel: 'Shopee', status: 'pago', total: 89.90, created_at: '2025-03-07 16:45:00' },
                { id: 15, order_number: 'PED-015', client_name: 'Gabriel Souza', channel: 'Nuvemshop', status: 'pendente', total: 229.80, created_at: '2025-03-08 08:00:00' },
            ];
            this.savePedidosData();
        }
    }

    private savePedidosData(): void {
        localStorage.setItem(this.pedidosStorageKey, JSON.stringify(this.pedidos));
    }
}

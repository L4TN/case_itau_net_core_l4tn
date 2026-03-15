export interface Canal {
  id: number;
  type: string;
  name: string;
  shortened_name: string;
  image_link: string;
  created_at: string;
  created_by: string;
}

export interface Cliente {
  id: number;
  name: string;
  storeName: string;
  whatsapp: string;
  email: string;
  currentBalance: string;
}

export interface Produto {
  id: number;
  name: string;
  price: number;
  quantity: number;
}

export interface Integracao {
  id: number;
  channel_id: number;
  client_id: number;
  store_name: string;
  store_description: string;
  api_key: string;
  client_key: string;
  client_secret: string;
  status: boolean;
  created_at: string;
  created_by: string;
  updated_at?: string;
  updated_by?: string;
  deleted_at?: string;
  deleted_by?: string;
}

export interface Pedido {
  id: number;
  order_number: string;
  client_name: string;
  channel: string;
  status: 'pago' | 'cancelado' | 'pendente';
  total: number;
  created_at: string;
}

export interface Usuario {
  id: number;
  name: string;
  store_name: string;
  person_type: string;        // 'PF' | 'PJ'
  document: string;
  ie_exempt: boolean;         // false=Não Isenta, true=Isenta
  ie: string;
  email: string;
  whatsapp: string;
  address_zip: string;
  address_street: string;
  address_number: number;
  address_complement: string;
  address_neighborhood: string;
  address_city: string;
  address_state: string;
  password: string;
  profile: string;            // 'interno' | 'cliente'
  status: boolean;            // true=Ativo, false=Inativo
  current_balance: number;
  is_blocked: boolean;        // false=Não, true=Bloqueado
  created_at: string;
  created_by: string;
  updated_at?: string;
  updated_by?: string;
  deleted_at?: string;
  deleted_by?: string;
}
export interface Group {
    name:string;
    connections: Connections[];
}
export interface Connections{
    connectionId:string;
    username: string;
}
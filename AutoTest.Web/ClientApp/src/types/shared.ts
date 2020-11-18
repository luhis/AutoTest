export interface EmergencyContact {
    readonly name: string;
    readonly phone: string;
}

export interface Vehicle {
    readonly make: string;
    readonly model: string;
    readonly year: number;
    readonly registration: string;
    readonly displacement: number;
}

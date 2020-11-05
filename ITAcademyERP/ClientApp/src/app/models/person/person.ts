import { Address } from "../address/address";

export interface Person {
    id: string;
    firstName: string;
    lastName: string;
    fullName: string;
    email: string;
    addresses: Address[];
    }
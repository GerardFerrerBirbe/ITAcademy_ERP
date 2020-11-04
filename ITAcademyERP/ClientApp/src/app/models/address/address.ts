import { Guid } from "guid-typescript";
import {AddressType} from "./addressType";

export interface Address {
  id: Guid;
  personId: string;
  name: string;
  type: AddressType;
}
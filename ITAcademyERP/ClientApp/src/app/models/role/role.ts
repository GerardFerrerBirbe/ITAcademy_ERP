import { RoleUser} from './roleUser';

export interface Role {
  id: string;
  name: string;
  roleUsers: RoleUser[];
}
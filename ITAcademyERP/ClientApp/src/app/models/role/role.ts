import { RoleUser} from './roleUser';

export interface Role {
  roleId: string;
  roleName: string;
  roleUsers: RoleUser[];
}
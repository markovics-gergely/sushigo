import { AppRole } from "./user.models";

/**
 * ACL Type.
 */
interface AclType {
    [name: string]: (AppRole | '*')[];
}

/**
 * ACL Value.
 */
export const ACL: AclType = {
    home: ['Classic'],
};
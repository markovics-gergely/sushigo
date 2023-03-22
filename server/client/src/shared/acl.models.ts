import { AppRole } from "./user.models";

interface AclType {
    [name: string]: (AppRole | '*')[];
}

export const ACL: AclType = {
    home: ['Classic'],
    store: ['Classic'],
    'store.claim': ['Classic'],
    lobby: ['Classic'],
};
import { AppRole } from "./user.models";

interface AclType {
    [name: string]: (AppRole | '*')[];
}

export const ACL: AclType = {
    'home': ['Classic'],
    'home.claim.party': ['CanClaimParty'],
    'shop': ['Party'],
    'lobby': ['Classic'],
    'lobby-list': ['Classic'],
    'game': ['Classic'],
};

export type AclPage = keyof typeof ACL;

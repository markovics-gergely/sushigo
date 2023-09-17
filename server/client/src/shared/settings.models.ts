export type Setting = 'logout' | 'edit-user' | 'remove-game' | 'delete-user';

export interface SettingsType {
  settings: Setting;
  icon: string;
}

export const Settings: SettingsType[] = [
  { settings: 'logout', icon: 'bx bx-log-out' },
  { settings: 'edit-user', icon: 'bx bx-edit' },
  { settings: 'remove-game', icon: 'bx bx-dice-6' },
  { settings: 'delete-user', icon: 'bx bx-user-x' }
];

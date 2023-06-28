export type Setting = 'logout' | 'edit-user';

export interface SettingsType {
  settings: Setting;
  icon: string;
}

export const Settings: SettingsType[] = [
  { settings: 'logout', icon: 'bx bx-log-out' },
  { settings: 'edit-user', icon: 'bx bx-edit' },
];

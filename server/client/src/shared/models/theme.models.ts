export type Theme = 'dark-theme' | 'light-theme';

export interface ThemeType {
  theme: Theme;
  icon: string;
}

export const Themes: ThemeType[] = [
  { theme: 'light-theme', icon: 'bx bx-sun' },
  { theme: 'dark-theme', icon: 'bx bx-moon' },
];

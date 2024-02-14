import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/pages/home/home.component';
import { AclGuard } from '../shared/guards/acl.guard';
import { StoreComponent } from './components/pages/store/store.component';
import { LobbyComponent } from './components/pages/lobby/lobby.component';
import { LobbyListComponent } from './components/pages/lobby-list/lobby-list.component';
import { HubGuard } from '../shared/guards/hub.guard';
import { loginGuard } from './login/guards/login.guard';
import { LobbyGuard } from './lobby/guards/lobby.guard';
import { gameGuard } from './game/guards/game.guard';
import { LoginPageComponent } from './login/login-page/login-page.component';
import { RegisterPageComponent } from './register/register-page/register-page.component';
import { GamePageComponent } from './game/game-page/game-page.component';

const routes: Routes = [
  {
    path: 'login',
    component: LoginPageComponent,
    data: { name: 'login', hub: [] },
    canActivate: [loginGuard, HubGuard]
  },
  {
    path: 'register',
    component: RegisterPageComponent,
    data: { name: 'register', hub: [] },
    canActivate: [loginGuard, HubGuard]
  },
  {
    path: 'home',
    component: HomeComponent,
    data: { name: 'home', hub: ['friend'] },
    canActivate: [AclGuard, HubGuard]
  },
  {
    path: 'shop',
    component: StoreComponent,
    data: { name: 'shop', hub: ['friend'] },
    canActivate: [AclGuard, HubGuard]
  },
  {
    path: 'lobby',
    component: LobbyListComponent,
    data: { name: 'lobby-list', hub: ['friend', 'lobbyList'] },
    canActivate: [AclGuard, HubGuard]
  },
  {
    path: 'lobby/:id',
    component: LobbyComponent,
    data: { name: 'lobby', hub: ['friend', 'lobby', 'lobbyList'] },
    canActivate: [AclGuard, LobbyGuard, HubGuard]
  },
  {
    path: 'game',
    component: GamePageComponent,
    data: { name: 'game', hub: ['friend', 'game', 'hand'] },
    canActivate: [AclGuard, gameGuard, HubGuard]
  },
  {
    path: '**',
    redirectTo: 'login',
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

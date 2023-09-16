import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/pages/home/home.component';
import { LoginComponent } from './components/pages/login/login.component';
import { RegisterComponent } from './components/pages/register/register.component';
import { AclGuard } from './guards/acl.guard';
import { StoreComponent } from './components/pages/store/store.component';
import { LobbyComponent } from './components/pages/lobby/lobby.component';
import { LobbyListComponent } from './components/pages/lobby-list/lobby-list.component';
import { HubGuard } from './guards/hub.guard';
import { LoginGuard } from './guards/login.guard';
import { LobbyGuard } from './guards/lobby.guard';
import { GameComponent } from './components/pages/game/game.component';
import { gameGuard } from './guards/game.guard';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    data: { name: 'login', hub: [] },
    canActivate: [LoginGuard, HubGuard]
  },
  {
    path: 'register',
    component: RegisterComponent,
    data: { name: 'register', hub: [] },
    canActivate: [LoginGuard, HubGuard]
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
    component: GameComponent,
    data: { name: 'game', hub: ['friend', 'game'] },
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

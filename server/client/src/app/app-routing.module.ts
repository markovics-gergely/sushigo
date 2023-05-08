import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AclGuard } from './guards/acl.guard';
import { StoreComponent } from './components/store/store.component';
import { LobbyComponent } from './components/lobby/lobby.component';
import { LobbyListComponent } from './components/lobby-list/lobby-list.component';
import { HubGuard } from './guards/hub.guard';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    data: { name: 'login', hub: [] },
    canActivate: [AclGuard, HubGuard]
  },
  {
    path: 'register',
    component: RegisterComponent,
    data: { name: 'register', hub: [] },
    canActivate: [AclGuard, HubGuard]
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
    canActivate: [AclGuard, HubGuard]
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

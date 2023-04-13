import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AclGuard } from './guards/acl.guard';
import { StoreComponent } from './components/store/store.component';

const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    data: { name: 'login' },
    canActivate: [AclGuard]
  },
  {
    path: 'register',
    component: RegisterComponent,
    data: { name: 'register' },
    canActivate: [AclGuard]
  },
  {
    path: 'home',
    component: HomeComponent,
    data: { name: 'home' },
    canActivate: [AclGuard]
  },
  {
    path: 'shop',
    component: StoreComponent,
    data: { name: 'shop' },
    canActivate: [AclGuard]
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

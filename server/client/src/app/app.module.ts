import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { CookieService } from 'ngx-cookie-service';
import { CanDirective } from './directives/can.directive';
import { LoadingComponent } from './components/overlay/loading/loading.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatBadgeModule } from '@angular/material/badge';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { CommonModule } from '@angular/common';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { HomeComponent } from './components/home/home.component';
import { FriendComponent } from './components/overlay/friend/friend.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { FriendAddDialogComponent } from './components/dialog/friend-add-dialog/friend-add-dialog.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    CanDirective,
    LoadingComponent,
    HomeComponent,
    FriendComponent,
    FriendAddDialogComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    MatSnackBarModule,
    MatBadgeModule,
    MatDialogModule,
    MatTooltipModule,
    MatCheckboxModule,
    CommonModule,
    MatExpansionModule,
    MatFormFieldModule,
    MatInputModule,
    JwtModule.forRoot({
      config: {},
    }),
    TranslateModule.forRoot({
      defaultLanguage: 'en',
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    })
  ],
  providers: [
    CookieService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }

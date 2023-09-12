import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/pages/login/login.component';
import { RegisterComponent } from './components/pages/register/register.component';
import { CookieService } from 'ngx-cookie-service';
import { CanDirective } from './directives/can.directive';
import { LoadingComponent } from './components/overlay/loading/loading.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
  HttpClient,
  HttpClientModule,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { JwtModule, JWT_OPTIONS } from '@auth0/angular-jwt';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatBadgeModule } from '@angular/material/badge';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { CommonModule, registerLocaleData } from '@angular/common';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { HomeComponent } from './components/pages/home/home.component';
import { FriendComponent } from './components/overlay/friend/friend.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { FriendAddDialogComponent } from './components/dialog/friend-add-dialog/friend-add-dialog.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { TokenService } from './services/token.service';
import { LanguageComponent } from './components/overlay/language/language.component';
import { RespMatGridTileDirective } from './directives/resp-mat-grid-tile.directive';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { UserComponent } from './components/pages/home/user/user.component';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { HistoryComponent } from './components/pages/home/history/history.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { StoreComponent } from './components/pages/store/store.component';
import { ConfirmComponent } from './components/dialog/confirm/confirm.component';
import { EditUserComponent } from './components/dialog/edit-user/edit-user.component';
import { ThemeComponent } from './components/overlay/theme/theme.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { LobbyComponent } from './components/pages/lobby/lobby.component';
import { RefreshComponent } from './components/dialog/refresh/refresh.component';
import { ImgPathPipe } from './pipes/img-path.pipe';
import { ChatComponent } from './components/pages/lobby/chat/chat.component';
import { LobbyListComponent } from './components/pages/lobby-list/lobby-list.component';
import { CreateLobbyComponent } from './components/dialog/create-lobby/create-lobby.component';
import { JoinLobbyComponent } from './components/dialog/join-lobby/join-lobby.component';
import localeHu from '@angular/common/locales/hu';
import { EditLobbyComponent } from './components/dialog/edit-lobby/edit-lobby.component';
import { MatRadioModule } from '@angular/material/radio';
import { SettingsComponent } from './components/overlay/settings/settings.component';
import { CardBaseComponent } from './components/pages/game/card/card-base/card-base.component';
import { CardWrapperComponent } from './components/pages/game/card/card-wrapper/card-wrapper.component';
import { PointCardComponent } from './components/pages/game/card/point-card/point-card.component';
import { GameComponent } from './components/pages/game/game.component';
import { HandComponent } from './components/pages/game/hand/hand.component';
import { NgScrollbarModule } from 'ngx-scrollbar';
import {MatListModule, MatNavList} from '@angular/material/list';
import { CardTypeSelectComponent } from './components/dialog/card-type-select/card-type-select.component';

export function jwtOptionsFactory(tokenService: TokenService) {
  return {
    tokenGetter: () => tokenService.token,
  };
}

registerLocaleData(localeHu);

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
    LanguageComponent,
    RespMatGridTileDirective,
    UserComponent,
    HistoryComponent,
    StoreComponent,
    ConfirmComponent,
    EditUserComponent,
    ThemeComponent,
    LobbyComponent,
    RefreshComponent,
    ImgPathPipe,
    ChatComponent,
    LobbyListComponent,
    CreateLobbyComponent,
    JoinLobbyComponent,
    EditLobbyComponent,
    SettingsComponent,
    CardBaseComponent,
    CardWrapperComponent,
    PointCardComponent,
    GameComponent,
    HandComponent,
    CardTypeSelectComponent,
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
    MatButtonToggleModule,
    MatGridListModule,
    MatCardModule,
    MatDividerModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatToolbarModule,
    MatRadioModule,
    NgScrollbarModule,
    MatListModule,
    JwtModule.forRoot({
      jwtOptionsProvider: {
        provide: JWT_OPTIONS,
        useFactory: jwtOptionsFactory,
        deps: [TokenService],
      },
    }),
    TranslateModule.forRoot({
      defaultLanguage: 'en',
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
    }),
  ],
  providers: [
    CookieService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    TokenService,
    {
      provide: LOCALE_ID,
      useValue: navigator.language,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

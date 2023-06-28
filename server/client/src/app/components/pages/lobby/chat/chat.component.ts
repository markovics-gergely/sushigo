import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { LoadingService } from 'src/app/services/loading.service';
import { LobbyService } from 'src/app/services/lobby.service';
import { MessageService } from 'src/app/services/message.service';
import { TokenService } from 'src/app/services/token.service';
import { IMessageDTO, IMessageViewModel } from 'src/shared/message.models';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ChatComponent implements OnInit {
  @Input() public lobbyId: string | undefined;
  private _messages: IMessageViewModel[] = [];
  private _messageForm: FormGroup | undefined;

  constructor(
    private messageService: MessageService,
    private lobbyService: LobbyService,
    private tokenService: TokenService,
    private loadingService: LoadingService,
    ) {}

  ngOnInit(): void {
    this._messageForm = new FormGroup({
      text: new FormControl(''),
    });
    this.lobbyService.lobbyEventEmitter.subscribe({
      next: (lobby) => {
        if (lobby) {
          this.loadingService.start();
          this.messageService.getMessages(lobby.id).subscribe({
            next: (messages: IMessageViewModel[]) => {
              this._messages = messages;
            }
          }).add(() => {
            this.loadingService.stop();
          });
        }
      }
    });
    this.messageService.messageEventEmitter.subscribe({
      next: (message) => {
        if (message) {
          this._messages.unshift(message);
          document.getElementById('msgwrapper')?.scroll({
            behavior: 'smooth',
            top: 0,
          });
        }
      }
    });
  }

  public get messages(): IMessageViewModel[] {
    return this._messages;
  }

  public get messageForm(): FormGroup | undefined {
    return this._messageForm;
  }

  public get valid(): boolean {
    return this.messageForm?.value.text || false;
  }

  public sendMessage(): void {
    if (!this.messageForm?.valid || !this.lobbyId) { return; }
    this.loadingService.start();
    const messageDTO = this.messageForm.value as IMessageDTO;
    messageDTO.lobbyId = this.lobbyId;
    this.messageService.addMessage(messageDTO).subscribe().add(() => {
      this.messageForm?.reset();
      this.loadingService.stop();
    });
  }

  public get timezone(): string {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
  }

  public ownMessage(message: IMessageViewModel): boolean {
    return message.userName === this.tokenService.userName;
  }
}

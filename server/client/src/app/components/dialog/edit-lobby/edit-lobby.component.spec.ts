import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditLobbyComponent } from './edit-lobby.component';

describe('EditLobbyComponent', () => {
  let component: EditLobbyComponent;
  let fixture: ComponentFixture<EditLobbyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditLobbyComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditLobbyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

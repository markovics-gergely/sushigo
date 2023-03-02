import { Component, OnInit } from '@angular/core';
import { FriendService } from 'src/app/services/friend.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(private friendService: FriendService) { }

  ngOnInit(): void {
    
  }
}

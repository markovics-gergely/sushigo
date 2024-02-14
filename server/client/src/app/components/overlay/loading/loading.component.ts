import { Component } from '@angular/core';
import { LoadingService } from 'src/shared/services/loading.service';

@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.scss']
})
export class LoadingComponent {

  constructor(private loadingService: LoadingService) { }

  get loading() {
    return this.loadingService.load;
  }
}

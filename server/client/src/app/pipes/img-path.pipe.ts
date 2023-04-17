import { Pipe, PipeTransform } from '@angular/core';
import { TokenService } from '../services/token.service';
import { environment } from 'src/environments/environment';

@Pipe({
  name: 'imgPath'
})
export class ImgPathPipe implements PipeTransform {

  constructor(private tokenService: TokenService) { }

  transform(value: string | undefined): string | undefined {
    if (!value || !(value.toLowerCase().endsWith('.png') || value.toLowerCase().endsWith('.jpg'))) return undefined;
    return `${environment.baseUrl}${value}?token=${this.tokenService.token}`;
  }
}

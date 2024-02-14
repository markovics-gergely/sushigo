import { HttpClient } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export abstract class BaseServiceService {
  protected abstract readonly basePath: string;
  protected readonly client: HttpClient;

  constructor(injector: Injector) {
    this.client = injector.get(HttpClient);
  }

  protected get baseUrl(): string {
    return `${environment.baseUrl}/${this.basePath}`;
  }

  /**
   * Generate form data from object
   * @param obj Object to transform
   * @returns FormData generated
   */
  protected getFormData(obj: any): FormData {
    return Object.keys(obj).reduce((formData, key) => {
      formData.append(key, obj[key]);
      return formData;
    }, new FormData());
  }
}

import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FolderService {
  baseUrl: string = 'https://localhost:5000/api/';
  private http = inject(HttpClient);

  createFolder(folderName: string) {
    return this.http.post(this.baseUrl + 'folders', { folderName }, { withCredentials: true })
  }
}

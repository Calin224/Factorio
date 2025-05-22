import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import {Folder} from '../../shared/models/folder';

@Injectable({
  providedIn: 'root'
})
export class FolderService {
  baseUrl: string = 'https://localhost:5000/api/';
  private http = inject(HttpClient);

  createFolder(folderName: string) {
    return this.http.post<Folder>(this.baseUrl + 'folders', { folderName }, { withCredentials: true });
  }

  getAllFolders(){
    return this.http.get<Folder[]>(this.baseUrl + 'folders/all-folders', { withCredentials: true });
  }
}

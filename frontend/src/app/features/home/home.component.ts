import { Component } from '@angular/core';
import { FolderComponent } from "../folder/folder.component";

@Component({
  selector: 'app-home',
  imports: [FolderComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}

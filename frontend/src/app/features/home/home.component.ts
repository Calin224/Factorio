import { Component } from '@angular/core';
import {AllFoldersComponent} from '../folder/all-folders/all-folders.component';

@Component({
  selector: 'app-home',
  imports: [AllFoldersComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}

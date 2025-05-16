import { Component, inject } from '@angular/core';
import { Dialog } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { FolderService } from '../../core/services/folder.service';

@Component({
  selector: 'app-folder',
  imports: [Dialog, ButtonModule, InputTextModule, ReactiveFormsModule],
  templateUrl: './folder.component.html',
  styleUrl: './folder.component.css'
})
export class FolderComponent {
  private folderService = inject(FolderService);
  private fb = new FormBuilder();

  folderForm = this.fb.group({
    folderName: ['', Validators.required]
  })

  visible: boolean = false;

  showDialog() {
      this.visible = true;
  }

  onSubmit(){
    let folderName = this.folderForm.get('folderName')!.value;

    if(!folderName){
      return;
    }

    this.folderService.createFolder(folderName).subscribe({
      next: res => {
        this.visible = false;
      }
    })
  }
}

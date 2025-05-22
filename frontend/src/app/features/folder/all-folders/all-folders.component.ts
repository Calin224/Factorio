import { Component, inject, OnInit } from '@angular/core';
import { FolderService } from '../../../core/services/folder.service';
import { Folder } from '../../../shared/models/folder';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Button } from 'primeng/button';
import { Dialog } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import {DatePipe} from '@angular/common';

@Component({
  selector: 'app-all-folders',
  standalone: true,
  imports: [
    Button,
    Dialog,
    InputTextModule,
    ReactiveFormsModule,
    ToastModule,
    DatePipe
  ],
  templateUrl: './all-folders.component.html',
  styleUrls: ['./all-folders.component.css'],
  providers: [MessageService]
})
export class AllFoldersComponent implements OnInit {
  private folderService = inject(FolderService);
  private fb = inject(FormBuilder);
  private messageService = inject(MessageService);

  folders: Folder[] = [];
  visible: boolean = false;
  folderForm: FormGroup;

  constructor() {
    this.folderForm = this.fb.group({
      folderName: ['', [Validators.required, Validators.minLength(3)]]
    });
  }

  ngOnInit(): void {
    this.loadFolders();
  }

  loadFolders() {
    this.folderService.getAllFolders().subscribe({
      next: (flds) => {
        this.folders = flds;
      },
      error: (err) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to load folders'
        });
        console.error('Error loading folders:', err);
      }
    });
  }

  showDialog() {
    this.folderForm.reset();
    this.visible = true;
  }

  onSubmit() {
    if (this.folderForm.invalid) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Invalid Input',
        detail: 'Please enter a valid folder name'
      });
      return;
    }

    const folderName = this.folderForm.get('folderName')?.value;
    this.folderService.createFolder(folderName).subscribe({
      next: () => {
        this.loadFolders();
        this.visible = false;
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Folder created successfully'
        });
      },
      error: (err) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to create folder'
        });
        console.error('Error creating folder:', err);
      }
    });
  }
}

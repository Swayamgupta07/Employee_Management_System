import { Component, EventEmitter, Output, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { ImageCropperComponent, ImageCroppedEvent } from 'ngx-image-cropper';

@Component({
  selector: 'app-profile-picture-modal',
  standalone: true,
  imports: [CommonModule, ImageCropperComponent],
  templateUrl: './profile-picture-modal.html',
  styleUrls: ['./profile-picture-modal.css']
})
export class ProfilePictureModalComponent {
  authService = inject(AuthService);

  @Output() closed = new EventEmitter<void>();

  fallBackUrl = signal<string>(this.authService.currentUser()?.profilePictureUrl || "data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAxMDAgMTAwIj48Y2lyY2xlIGN4PSI1MCIgY3k9IjUwIiByPSI1MCIgZmlsbD0iIzJjM2U1MCIgLz48Y2lyY2xlIGN4PSI1MCIgY3k9IjQwIiByPSIyMCIgZmlsbD0iI2VjZjBmMSIgLz48cGF0aCBkPSJNIDIwIDkwIFEgNTAgNjAgODAgOTAiIGZpbGw9Im5vbmUiIHN0cm9rZT0iI2VjZjBmMSIgc3Ryb2tlLXdpZHRoPSIxNSIgc3Ryb2tlLWxpbmVjYXA9InJvdW5kIiAvPjwvc3ZnPg==");
  imageChangedEvent: any = '';
  croppedImage = signal<string | null>(null);
  finalBase64Data = signal<string | null>(null);
  loading = signal(false);

  onFileSelected(event: any): void {
    if (event.target.files && event.target.files.length) {
      this.imageChangedEvent = event;
      this.finalBase64Data.set(null); 
    }
  }

  imageCropped(event: ImageCroppedEvent) {
    if (event.objectUrl) {
      this.croppedImage.set(event.objectUrl);
    }

    if (event.base64) {
      this.finalBase64Data.set(event.base64);
    } else if (event.blob) {
      const reader = new FileReader();
      reader.readAsDataURL(event.blob);
      reader.onloadend = () => {
        this.finalBase64Data.set(reader.result as string);
      };
    }
  }

  saveImage(): void {
    const base64 = this.finalBase64Data();
    if (!base64 || !base64.startsWith('data:image')) {
        alert("Wait a second for the crop to process, then try again.");
        return;
    }

    this.loading.set(true);
    this.authService.uploadProfilePicture(base64).subscribe({
      next: (res) => {
        this.loading.set(false);
        if (res.success) {
          this.close();
        }
      },
      error: () => {
        this.loading.set(false);
        alert('Failed to save profile picture.');
      }
    });
  }

  setDefault(avatarId: 'male' | 'female'): void {
    this.loading.set(true);
    this.authService.setDefaultProfilePicture(avatarId).subscribe({
      next: (res) => {
        this.loading.set(false);
        if (res.success) {
          this.close();
        }
      },
      error: () => {
        this.loading.set(false);
        alert('Failed to set default avatar.');
      }
    });
  }

  close(): void {
    this.imageChangedEvent = '';
    this.croppedImage.set(null);
    this.closed.emit();
  }
}

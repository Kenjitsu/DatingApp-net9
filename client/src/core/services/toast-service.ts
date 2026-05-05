import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private router = inject(Router);
  
  constructor() { 
    this.createToastContainer();
  }
  
  private createToastContainer() {
    if (!document.getElementById('toast-container')) {
      const container = document.createElement('div');
      container.id = 'toast-container';
      container.className = 'toast toast-bottom toast-end z-50';

      document.body.appendChild(container);
    }
  }

  private createToastElement(message: string, alertClass: string, duration = 5000,
      avatar?: string, route?: string, messageContent?: string) {
    const toastContainer = document.getElementById('toast-container');

    if (!toastContainer) return;

    const toast = document.createElement('div');
   toast.classList.add('alert', alertClass, 'shadow-lg', 'flex',
    'items-center', 'gap-3', 'cursor-pointer', 'w-full', 'max-w-md');

if (route) { 
    toast.addEventListener('click', () => this.router.navigateByUrl(route));
}
    
if (avatar || messageContent) {
    toast.innerHTML = `
    <div class="avatar pt-1">
        <div class="w-10 h-10 rounded-full shrink-0">
        <img src="${avatar || '/user.png'}" alt="Avatar" />
        </div>
    </div>
    <div class="flex-1 min-w-0 text-left overflow-hidden">
        <h3 class="text-sm font-semibold truncate">
        <span class="font-bold">${message}</span> send you a message!
        </h3>
        <p class="text-xs opacity-80 mt-1 truncate leading-tight">${messageContent || ''}</p>
    </div>
    <button class="close-btn btn btn-ghost btn-xs btn-circle ml-1 shrink-0">✕</button>
    `;
} else {
    // Diseño de toast normal
    toast.innerHTML = `
    <div class="flex-1 wrap-break-word">
        <!-- 'message' actúa aquí como la notificación regular -->
        <span>${message}</span>
    </div>
    <button class="close-btn btn btn-sm btn-ghost ml-4 shrink-0">✕</button>
    `;
}

    toast.querySelector('button')?.addEventListener('click', () => {
      toastContainer.removeChild(toast);
    });

    toastContainer.append(toast);

    setTimeout(() => {
      if (toastContainer.contains(toast)) {
        toastContainer.removeChild(toast);
      }
    }, duration);
  }

  success(message: string, duration?: number, avatar?: string, route?: string, messageContent?: string) {
    this.createToastElement(message, 'alert-success', duration, avatar, route, messageContent);
  }

  error(message: string, duration?: number, avatar?: string, route?: string, messageContent?: string) {
    this.createToastElement(message, 'alert-error', duration, avatar, route, messageContent);
  }

  warning(message: string, duration?: number, avatar?: string, route?: string, messageContent?: string) {
    this.createToastElement(message, 'alert-warning', duration, avatar, route, messageContent);
  }

  info(message: string, duration?: number, avatar?: string, route?: string, messageContent?: string) {
    this.createToastElement(message, 'alert-info', duration, avatar, route, messageContent);
  }
}

import { Component, computed, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Member } from '../../../types/member';
import { AgePipe } from '../../../core/pipes/age.pipe';
import { LikesService } from '../../../core/services/likes-service';
import { PresenceService } from '../../../core/services/presence-service';

@Component({
    selector: 'app-member-card',
    imports: [RouterLink, AgePipe],
    templateUrl: './member-card.component.html',
    styleUrl: './member-card.component.css'
})
export class MemberCardComponent {
  private likesService = inject(LikesService);
  private presenceService = inject(PresenceService);
  member = input.required<Member>();
  protected hasLiked = computed(() => this.likesService.likeIds().includes(this.member().id));
  protected isOnline = computed(() => this.presenceService.onlineUsers().includes(this.member().id));

  toggleLike(event: Event) {
    event.stopPropagation();
    this.likesService.toggleLike(this.member().id)
  }
}

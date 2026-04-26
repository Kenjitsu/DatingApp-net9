import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Member } from '../../../types/member';
import { filter } from 'rxjs';
import { AgePipe } from '../../../core/pipes/age.pipe';
// import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';

@Component({
    selector: 'app-member-detail',
    imports: [RouterLink, RouterLinkActive, RouterOutlet, AgePipe],
    templateUrl: './member-detail.component.html',
    styleUrl: './member-detail.component.css'
})
  
export class MemberDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  protected member = signal<Member | undefined>(undefined);
  protected title = signal<string | undefined>('Profile');
  // images: GalleryItem[] = [];

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => this.member.set(data['member'])
    })
    this.title.set(this.route.firstChild?.snapshot.title)

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe({
      next: () => {
        this.title.set(this.route.firstChild?.snapshot.title)
      }
    })
  }
}

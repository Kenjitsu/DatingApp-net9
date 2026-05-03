import { Component, inject, OnInit, signal } from '@angular/core';
import { LikesService } from '../../core/services/likes-service';
import { Member } from '../../types/member';
import { MemberCardComponent } from "../members/member-card/member-card.component";

@Component({
    selector: 'app-lists',
    imports: [MemberCardComponent],
    templateUrl: './lists.component.html',
    styleUrl: './lists.component.css'
})
export class ListsComponent implements OnInit {
    private likesService = inject(LikesService);
    protected members = signal<Member[]>([]);
    protected predicate = 'liked';

    tabs = [
        { label: 'Liked', value: 'liked' },
        { label: 'Liked me', value: 'likedBy' },
        { label: 'Mutual', value: 'mutual'}
    ]

    ngOnInit(): void {
        this.loadLikes();
    }

    setPredicate(predicate: string) {
        if (this.predicate !== predicate) {
            this.predicate = predicate;
            this.loadLikes();
        }
    }

    loadLikes() {
        this.likesService.getlikes(this.predicate).subscribe({
            next: members => this.members.set(members)
        });
    }
}

import { Component, inject, OnInit } from '@angular/core';
import { MemberCardComponent } from '../member-card/member-card.component';
import { MembersService } from '../../../core/services/members.service';
import { Observable } from 'rxjs';
import { Member } from '../../../types/member';
import { AsyncPipe } from '@angular/common';

@Component({
    selector: 'app-member-list',
    imports: [MemberCardComponent, AsyncPipe],
    templateUrl: './member-list.component.html',
    styleUrl: './member-list.component.css'
})
export class MemberListComponent /*implements OnInit*/ {
  private memberService = inject(MembersService);
  protected members$: Observable<Member[]>;

  constructor() {
    this.members$ = this.memberService.getMembers();
  }

  // ngOnInit(): void {
  //   if(this.memberService.members().length === 0) this.loadMembers();
  // }

  // loadMembers() {
  //   this.memberService.getMembers();
  // }
}

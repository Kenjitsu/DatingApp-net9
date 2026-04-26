import { Component, HostListener, inject, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { PhotoEditorComponent } from "../photo-editor/photo-editor.component";
import { Member } from '../../../types/member';

@Component({
    selector: 'app-member-edit',
    imports: [FormsModule/*, PhotoEditorComponent*/],
    templateUrl: './member-edit.component.html',
    styleUrl: './member-edit.component.css'
})
export class MemberEditComponent {
  // @ViewChild('editForm') editForm?: NgForm;
  // @HostListener('window:beforeunload', ['$event']) notify($event:any) {
  //   if (this.editForm?.dirty) {
  //     $event.returnValue = true;
  //   }
  // }
  // member?: Member;
  // private accountService = inject(AccountService);
  // private memberService = inject(MembersService);
  // private toastr = inject(ToastrService);

  // ngOnInit(): void {
  //   this.loadMember();
  // }

  // loadMember() {
  //   const user = this.accountService.currentUser();

  //   if (!user) return;
    
  //   this.memberService.getMember(user.username).subscribe({
  //     next: (member) => this.member = member,
  //   });
  // }

  // updateMember() {
  //   this.memberService.updateUser(this.editForm?.value).subscribe({
  //     next: _ => {
  //       this.toastr.success('Profile updated successfully');
  //       this.editForm?.reset(this.member);
  //     }
  //   })
  // }

  // onMemberChanged(event: Member) {
  //    this.member = event;
  // }
}

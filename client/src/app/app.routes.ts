import { Routes } from '@angular/router';
import { HomeComponent } from '../features/home/home.component';
import { MemberListComponent } from '../features/members/member-list/member-list.component';
import { MemberDetailComponent } from '../features/members/member-detail/member-detail.component';
import { ListsComponent } from '../features/lists/lists.component';
import { MessagesComponent } from '../features/messages/messages.component';
import { authGuard } from '../core/guards/auth.guard';
import { TestErrorsComponent } from '../features/errors/test-errors/test-errors.component';
import { NotFoundComponent } from '../shared/errors/not-found/not-found.component';
import { ServerErrorComponent } from '../shared/errors/server-error/server-error.component';
import { MemberEditComponent } from '../features/members/member-edit/member-edit.component';
import { preventUnsavedChangesGuard } from '../core/guards/prevent-unsaved-changes.guard';
import { MemberProfile } from '../features/members/member-profile/member-profile';
import { MemberPhotos } from '../features/members/member-photos/member-photos';
import { MemberMessages } from '../features/members/member-messages/member-messages';
import { memberResolver } from '../features/members/member.resolver';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [ authGuard ],
        children: [
            { path: 'members', component: MemberListComponent },
            {
                path: 'members/:id',
                resolve: { member: memberResolver },
                runGuardsAndResolvers: 'always',
                component: MemberDetailComponent,
                children: [
                    { path: '', redirectTo: 'profile', pathMatch: 'full' },
                    { path: 'profile', component: MemberProfile, title: 'Profile' },
                    { path: 'photos', component: MemberPhotos, title: 'Photos' },
                    { path: 'messages', component: MemberMessages, title: 'Messages' },
                ]
            },
            { path: 'member/edit', component: MemberEditComponent/*, canDeactivate: [preventUnsavedChangesGuard] */},
            { path: 'lists', component: ListsComponent },
            { path: 'messages', component: MessagesComponent },
        ]
    },
    {path: 'errors', component: TestErrorsComponent},
    {path: 'not-found', component: NotFoundComponent},
    {path: 'server-error', component: ServerErrorComponent},
    {path: '**', component: NotFoundComponent, /*pathMatch: "full"*/},
]

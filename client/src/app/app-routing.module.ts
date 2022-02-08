import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

// making the route urls and telling the router which component the 
// route corresponds to
const routes: Routes = [
  // '' means that the route will be {baseUrl}/
  {path: '', component: HomeComponent},
  // making a group of routes to apply the authgurad to all of them
  {
    // the group doesnt need a route path
    path: '',
    // this is so that the guard applies to all the routes in the group
    runGuardsAndResolvers: "always",
    // the actual guard assignment
    canActivate: [AuthGuard],
    // adding all the members of the group as the children so 
    children: [
      // this is {baseUrl}/members and it leads to the Members List component
      {path: 'members', component: MemberListComponent},
      // this is {baseUrl}/members/{id} and it leads to the MembersDetail component
      {path: 'members/:id', component: MemberDetailComponent},
      // this is {baseUrl}/lists and it leads to the Lists component
      {path: 'lists', component: ListsComponent},
      // this is {baseUrl}/messages and it leads to the Messages component
      {path: 'messages', component: MessagesComponent},
    ]
  },
  // this is anything and it leads to the HomeComponent for now and the path match
  // is because of the fact that when the router checks the url, it tries to match
  // the input with the route and if it doesn't match as in this case, it will end 
  // in an endless loop if we dont enter the pathMatch: "full" parameter
  {path: '**', component: HomeComponent, pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

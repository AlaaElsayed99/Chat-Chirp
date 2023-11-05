import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabDirective, TabsModule, TabsetComponent } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';
import {  ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/member';
import { MemberService } from 'src/app/_services/member.service';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/messages';
import { PresenceService } from 'src/app/_services/presence.service';
import { AccountService } from 'src/app/_services/account.service';
import { User } from 'src/app/_models/user';
import { take } from 'rxjs';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css'],
  standalone:true,
  imports:[CommonModule,TabsModule,GalleryModule,TimeagoModule,MemberMessagesComponent]
})
export class MemberDetailsComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', {static:true}) memberTabs?:TabsetComponent; 
   member: Member= {} as Member;
   images:GalleryItem[]=[];
   activeTab?:TabDirective;
   messages:Message[]=[];
   users?:User;
  constructor(private memberService:MemberService, private route:ActivatedRoute,
    private toastr:ToastrService,private messageService:MessageService,
     public peresenceService:PresenceService,private accountService:AccountService) {
    this.accountService.currnetUser$.pipe(take(1)).subscribe({
      next:user=> {
        if(user) this.users=user;
      }
    })
  }
  ngOnDestroy(): void {
    this.messageService.stropHubConnection();
  }
  ngOnInit(): void {
    this.route.data.subscribe({
      next: data=> this.member=data['member']
    })
    this.route.queryParams.subscribe({
      next:params=> {
        params['tab']&&this.selectTab(params['tab']);
      }
    })
    this.getImages()
  }
onTabActivated(data:TabDirective){
  this.activeTab=data;
  if(this.activeTab.heading==='Messages'&& this.users){
    // this.loadMessages();
    this.messageService.createHubConnection(this.users,this.member.userName)
  } else{
    this.messageService.stropHubConnection();
  }
}

selectTab(heading:string){
  if(this.memberTabs){
    this.memberTabs.tabs.find(x=>x.heading===heading)!.active=true;
  }
}

// loadMember(){
//   const username=this.route.snapshot.paramMap.get('username');
//   if(!username) return;
//   this.memberService.getMember(username).subscribe({
//     next:member =>{ this.member = member,
      
    
//     },
//   })
// }
loadMessages(){
  if(this.member){
    this.messageService.getMessageThread(this.member.userName).subscribe({
      next:messages=>this.messages=messages
    })
  }
}
getImages(){
  if(!this.member) return;
  for(const photo of this.member?.photos){
    this.images.push(new ImageItem({src:photo.url,thumb:photo.url}))
  }
}
addLike(member:Member){
  this.memberService.addLike(member.userName).subscribe({
    next:()=>this.toastr.success("you have liked "+ member.knownAs),
    
  })
}
}

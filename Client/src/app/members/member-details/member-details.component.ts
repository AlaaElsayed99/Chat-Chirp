import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
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

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css'],
  standalone:true,
  imports:[CommonModule,TabsModule,GalleryModule,TimeagoModule,MemberMessagesComponent]
})
export class MemberDetailsComponent implements OnInit {
  @ViewChild('memberTabs', {static:true}) memberTabs?:TabsetComponent; 
   member: Member= {} as Member;
   images:GalleryItem[]=[];
   activeTab?:TabDirective;
   messages:Message[]=[]
  constructor(private memberService:MemberService, private route:ActivatedRoute,private toastr:ToastrService,private messageService:MessageService) {
    
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
  if(this.activeTab.heading==='Messages'){
    this.loadMessages();
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

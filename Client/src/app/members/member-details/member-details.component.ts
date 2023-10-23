import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';
import { Member } from 'src/app/_models/member';
import { MemberService } from 'src/app/_services/member.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css'],
  standalone:true,
  imports:[CommonModule,TabsModule,GalleryModule,TimeagoModule]
})
export class MemberDetailsComponent implements OnInit {
   member: Member|undefined;
   images:GalleryItem[]=[];
  constructor(private memberService:MemberService, private route:ActivatedRoute) {
    
  }
  ngOnInit(): void {
    this.loadMember();
  }
loadMember(){
  const username=this.route.snapshot.paramMap.get('username');
  if(!username) return;
  this.memberService.getMember(username).subscribe({
    next:member =>{ this.member = member,
      this.getImages()
    
    },
  })
}
getImages(){
  if(!this.member) return;
  for(const photo of this.member?.photos){
    this.images.push(new ImageItem({src:photo.url,thumb:photo.url}))
  }
}
}

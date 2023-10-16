import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  Members:Member[]=[];
  constructor(private memberService:MemberService) {
    
  }
  ngOnInit(): void {
   this.getMembers();
  }
  getMembers(){
    this.memberService.getMembers().subscribe({
    next:members=>this.Members=members,
    error:err=>console.log(err),
  
  })
  }

}

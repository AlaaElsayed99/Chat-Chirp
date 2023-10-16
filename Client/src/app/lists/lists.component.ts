import { Component, OnInit } from '@angular/core';
import { MemberService } from '../_services/member.service';
import { Member } from '../_models/member';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit{
  Members:Member[]=[]
  constructor(private memberSrvice:MemberService) {
    
  }
  ngOnInit(): void {
   this.getMembers();
  }
  getMembers(){
    this.memberSrvice.getMembers().subscribe({
    next:members=>this.Members=members,
    error:err=>console.log(err),
  
  })
  }

}

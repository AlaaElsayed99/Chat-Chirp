import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/messages';
import { Pagination } from '../_models/Pagination';
import { MessageService } from '../_services/message.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages:Message[] |undefined;
  pagination : Pagination |undefined;
  container = 'Unread';
  pageNumber= 1; 
  pageSize= 5;
  loading = false;

  constructor(private messageService: MessageService, private toastr: ToastrService) {
    
  }
  ngOnInit(): void {
    this.loadMessages();
  }
  loadMessages(){
    this.loading=true
    this.messageService.getMessages(this.pageNumber,this.pageSize,this.container).subscribe({
      next: response =>{
        this.messages=response.result;
        this.pagination=response.pagination;
        this.loading=false;

      }
    })
  }

  deleteMessage(id:number){
    this.messageService.deleteMessage(id).subscribe({
      next:_ => this.messages?.splice(this.messages.findIndex(m => m .id === id), 1)&&
      this.toastr.success("message is deleted")

      
      
    })
  }

  pageChanged(event:any){
    if(this.pageNumber!==event.page){
      this.pageNumber=event.page;
      this.loadMessages();
    }
  }




}

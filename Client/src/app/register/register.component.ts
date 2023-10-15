import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  
  @Output() cancelRegister=new EventEmitter();
  model:any={}
  constructor(private http:HttpClient, private accountService:AccountService, private toastr:ToastrService) {
    
    
  }
  ngOnInit(): void {
    
  }
  register()
  {
    this.accountService.register(this.model).subscribe({
      next:()=>{
        this.cancel();
      },
      error:err=> this.toastr.error(err.error),
    })
  }
  cancel(){
    this.cancelRegister.emit(false);
  }

}

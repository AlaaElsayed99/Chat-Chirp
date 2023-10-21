import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  
  @Output() cancelRegister=new EventEmitter();
  registerForm:FormGroup=new FormGroup({});
  maxDate: Date=new Date();
  valaidationError:string|undefined;

  constructor(private http:HttpClient, private accountService:AccountService, private toastr:ToastrService,
     private fb:FormBuilder, private router: Router) {
    
    
  }
  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() -18);
  }
  initializeForm(){
    this.registerForm=this.fb.group({
      // before th edit fb FormBuilder
      // this.registerForm= new FormGroup{
      // username:new FormControl('', Validators.required),
      // password:new FormControl('',[ Validators.required,Validators.maxLength(8),Validators.minLength(4) ]),
      // confirmPassword:new FormControl('',[ Validators.required,this.matchValues('password') ]),
      // }
      gender:['male'],
      username:['', Validators.required],
      knownAs:['', Validators.required],
      dateOfBirth:['', Validators.required],
      city:['', Validators.required],
      country:['', Validators.required],
      password:['',[ Validators.required,Validators.maxLength(12),Validators.minLength(4) ]],
      confirmPassword:['',[ Validators.required,this.matchValues('password') ]],
    })
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: ()=> this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }
  matchValues(matchTo:string) :ValidatorFn{
    return(control:AbstractControl)=>{
      return control.value===control.parent?.get(matchTo)?.value ?null : {notMatching: true}
    }
  }
  register()
  {
    const dob = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value);
    const values= {...this.registerForm.value,dateOfBirth:dob};
    this.accountService.register(values).subscribe({
      next:()=>{
        this.router.navigateByUrl('/members')
      },
      error:err=> {
        this.valaidationError=err;
      }
    })
   }
  cancel(){
    this.cancelRegister.emit(false);
  
   }
   private getDateOnly(dob:string|undefined){
    if(!dob) return;
    let theDob= new Date(dob);
    return new Date(theDob.setMinutes(theDob.getMinutes()-theDob.getTimezoneOffset()))
      .toISOString().slice(0,10);
   }

}

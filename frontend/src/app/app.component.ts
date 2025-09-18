import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { ApiService } from './services/api.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NgxMaskDirective],
  providers: [provideNgxMask()],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  private fb = inject(FormBuilder);
  private api = inject(ApiService);

  submitted = signal(false);
  savedMessage = signal<any | null>(null);
  captchaUrl = '';
  captchaError = false;

  form = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    email: [
      '',
      [Validators.required, Validators.pattern(/^[A-Za-z0-9._-]+@[a-z0-9-]+\.[a-z]{2,}$/)],
    ],
    phone: ['', [Validators.required, Validators.pattern(/^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$/)]],
    topic_id: [null as number | null, [Validators.required]],
    text: ['', [Validators.required, Validators.minLength(5)]],
    captchaInput: ['', [Validators.required, Validators.pattern(/^\d{5}$/)]],
  });

  topics = signal<{ id: number; name: string }[]>([]);

  ngOnInit() {
    this.api.getTopics().subscribe({
      next: (topics) => this.topics.set(topics),
      error: () => this.topics.set([]),
    });
    this.refreshCaptcha();
  }

  refreshCaptcha() {
    this.captchaUrl = this.api.getCaptchaUrl();
  }

  get emailCtrl(): AbstractControl<any, any> {
    return this.form.get('email') as AbstractControl<any, any>;
  }

  get phoneCtrl(): AbstractControl<any, any> {
    return this.form.get('phone') as AbstractControl<any, any>;
  }

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const value = this.form.getRawValue();
    this.captchaError = false;
    this.api.validateCaptcha(value.captchaInput!).subscribe({
      next: () => {
        const payload = {
          name: value.name!,
          email: value.email!,
          phone: value.phone!,
          topic_id: value.topic_id!,
          text: value.text!,
          captcha: value.captchaInput!,
        };
        this.api.createMessage(payload).subscribe((res) => {
          this.savedMessage.set(res);
          this.submitted.set(true);
        });
      },
      error: () => {
        this.captchaError = true;
        this.refreshCaptcha();
      },
    });
  }
}

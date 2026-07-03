import { Component, EventEmitter, Input, OnInit, Output, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { AttendanceService } from '../../services/attendance.service';
import { Attendance } from '../../models/attendance.model';

@Component({
  selector: 'app-attendance-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './attendance-form.component.html',
  styles: [`
    /* Custom stepper for Employee ID */
    .emp-stepper {
      display: flex;
      align-items: center;
      border: 1.5px solid rgba(255,255,255,0.1);
      border-radius: 8px;
      overflow: hidden;
      background: var(--bg-raised, #1A2233);
    }
    .emp-stepper input[type=number] {
      -moz-appearance: textfield;
      border: none;
      background: transparent;
      color: #F0F4FF;
      text-align: center;
      flex: 1;
      font-size: 0.95rem;
      font-weight: 600;
      padding: 8px 4px;
    }
    .emp-stepper input[type=number]::-webkit-inner-spin-button,
    .emp-stepper input[type=number]::-webkit-outer-spin-button { -webkit-appearance: none; }
    .emp-stepper input[type=number]:focus { outline: none; }
    .stepper-btn {
      width: 38px; height: 38px;
      background: rgba(255,255,255,0.05);
      border: none;
      color: #94A3B8;
      font-size: 1.1rem;
      font-weight: 700;
      cursor: pointer;
      transition: background 0.15s;
      display: flex; align-items: center; justify-content: center;
      flex-shrink: 0;
      line-height: 1;
    }
    .stepper-btn:hover:not(:disabled) { background: rgba(0,201,167,0.15); color: #00C9A7; }
    .stepper-btn:disabled { opacity: 0.3; cursor: not-allowed; }

    /* Absent dimmed time fields */
    .time-section { transition: opacity 0.2s; }
    .time-section.absent { opacity: 0.4; pointer-events: none; }
  `]
})
export class AttendanceFormComponent implements OnInit {
  @Input() record: Attendance | null = null;
  @Output() close = new EventEmitter<void>();
  @Output() saved = new EventEmitter<void>();

  private fb     = inject(FormBuilder);
  private attSvc = inject(AttendanceService);

  form = this.fb.group({
    employeeId:   [1, [Validators.required, Validators.min(1)]],
    date:         ['', Validators.required],
    status:       ['Present', Validators.required],
    checkInTime:  ['', Validators.required],   // toggled dynamically
    checkOutTime: ['', Validators.required],   // toggled dynamically
    notes:        ['']
  });

  loading = signal(false);
  error   = signal('');

  get isAbsent(): boolean {
    return this.form.get('status')?.value === 'Absent';
  }

  ngOnInit(): void {
    // React to status changes
    this.form.get('status')!.valueChanges.subscribe(status => {
      this.updateTimeValidators(status ?? 'Present');
    });

    if (this.record) {
      this.form.patchValue({
        employeeId:   this.record.employeeId,
        date:         new Date(this.record.date).toISOString().split('T')[0],
        status:       this.record.status,
        checkInTime:  this.record.checkInTime,
        checkOutTime: this.record.checkOutTime,
        notes:        this.record.notes
      });
      this.form.controls.employeeId.disable();
      this.updateTimeValidators(this.record.status);
    }
  }

  /** Remove required validator for Absent, add for everything else */
  private updateTimeValidators(status: string): void {
    const checkIn  = this.form.get('checkInTime')!;
    const checkOut = this.form.get('checkOutTime')!;

    if (status === 'Absent') {
      checkIn.clearValidators();
      checkOut.clearValidators();
      checkIn.setValue('');
      checkOut.setValue('');
    } else {
      checkIn.setValidators(Validators.required);
      checkOut.setValidators(Validators.required);
    }

    checkIn.updateValueAndValidity();
    checkOut.updateValueAndValidity();
  }

  // ── Stepper helpers ─────────────────────────────────────
  stepUp(): void {
    const ctrl = this.form.get('employeeId')!;
    ctrl.setValue((Number(ctrl.value) || 0) + 1);
  }

  stepDown(): void {
    const ctrl = this.form.get('employeeId')!;
    const next = (Number(ctrl.value) || 1) - 1;
    if (next >= 1) ctrl.setValue(next);
  }

  onEmpIdInput(event: Event): void {
    const val = Number((event.target as HTMLInputElement).value);
    if (val < 1) this.form.get('employeeId')!.setValue(1);
  }

  // ── Submit ──────────────────────────────────────────────
  submit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }

    this.loading.set(true);
    this.error.set('');

    const raw = this.form.getRawValue();
    const absent = raw.status === 'Absent';

    const payload: any = {
      employeeId:   Number(raw.employeeId),
      date:         raw.date,
      checkInTime:  absent ? null : raw.checkInTime,
      checkOutTime: absent ? null : raw.checkOutTime,
      status:       raw.status,
      notes:        raw.notes || ''
    };

    if (this.record) payload.id = this.record.id;

    const request$ = this.record
      ? this.attSvc.update(this.record.id, payload)
      : this.attSvc.create(payload);

    request$.subscribe({
      next: () => this.saved.emit(),
      error: (err) => {
        this.error.set(err?.error?.message || 'Error saving attendance record');
        this.loading.set(false);
      }
    });
  }
}

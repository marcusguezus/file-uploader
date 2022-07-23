﻿import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
    selector: 'file-upload',
    templateUrl: 'file-upload.component.html'
   /* styleUrls: ['file-upload.component.css']*/
})

export class FileUploadComponent implements OnInit {
    public message: string;
    public progress: number;
    @Output() public onUploadFInished = new EventEmitter();

    // Inject service 
    constructor(private http: HttpClient) { }

    ngOnInit(): void {
    }

    public uploadFile = (files) => {
        if (files.length === 0) return;

        let fileToUpload = <File>files[0];
        const formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);

        this.http.post('https://localhost:44304/api/fileupload', formData, { reportProgress: true, observe: 'events' })
            .subscribe(event => {
                if (event.type === HttpEventType.UploadProgress) {
                    this.progress = Math.round(100 * event.loaded / event.total);
                }
                else if (event.type === HttpEventType.Response) {
                    this.message = 'Upload success.';
                    this.onUploadFInished.emit(event.body);
                }
            });
    }
}
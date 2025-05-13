export interface LocationResponseDTO {
     id: number;
     locationName: string;
     address: string;
     createdAt?: Date;
     lastModified?: Date;
     isDeleted: boolean;
     createdById?: string;
     lastModifiedById?: string;
 }
 
 export enum UserGender {
     Male = "Male",
     Female = "Female",
     Other = "Other"
 }
 
 export interface UserResponseDTO {
     id: string;
     fullName: string;
     userName: string;
     email: string;
     phoneNumber: string;
     gender: UserGender;
     birthDate?: Date;
     avatar?: string;
     balance?: number;
     role: string;
     isBanned: boolean;
     location: LocationResponseDTO;
 }
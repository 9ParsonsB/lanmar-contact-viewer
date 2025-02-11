export default interface Contact {
    id: string;
    firstName: string;
    lastName: string;
    company: string;
    phone: string;
    email: string;
    lastUpdated: string | Date;
}